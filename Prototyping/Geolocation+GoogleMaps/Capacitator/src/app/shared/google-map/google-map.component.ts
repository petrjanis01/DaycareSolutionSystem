import { Component, OnInit, Renderer2, ElementRef, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Plugins } from '@capacitor/core';
declare var google;

const { Geolocation, Network } = Plugins;

@Component({
  selector: 'google-map',
  templateUrl: './google-map.component.html',
  styleUrls: ['./google-map.component.scss'],
})
export class GoogleMapComponent implements OnInit {
  // Comes from config
  private apiKey = 'AIzaSyCrtbvWXbFWyv83J0T7wbH7elFLB4zUVnM';

  public map: any;
  public markers: any[] = [];
  private mapsLoaded = false;
  private networkHandler = null;

  constructor(private renderer: Renderer2, private element: ElementRef, @Inject(DOCUMENT) private document) { }

  ngOnInit() {
    this.init().then((res) => {
      console.log("Google Maps ready.")
    }, (err) => {
      console.log(err);
    });
  }

  private init(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.loadSDK().then((res) => {
        this.initMap().then((res) => {
          resolve(true);
        }, (err) => {
          reject(err);
        });
      }, (err) => {
        reject(err);
      });
    });
  }

  private loadSDK(): Promise<any> {

    console.log("Loading Google Maps SDK");

    return new Promise((resolve, reject) => {
      if (!this.mapsLoaded) {
        Network.getStatus().then((status) => {
          if (status.connected) {
            this.injectSDK().then((res) => {
              resolve(true);
            }, (err) => {
              reject(err);
            });
          } else {

            if (this.networkHandler == null) {
              this.networkHandler = Network.addListener('networkStatusChange', (status) => {

                if (status.connected) {
                  this.networkHandler.remove();
                  this.init().then((res) => {
                    console.log("Google Maps ready.")
                  }, (err) => {
                    console.log(err);
                  });
                }
              });
            }

            reject('Not online');
          }

        }, (err) => {

          // NOTE: navigator.onLine temporarily required until Network plugin has web implementation
          if (navigator.onLine) {
            this.injectSDK().then((res) => {
              resolve(true);
            }, (err) => {
              reject(err);
            });
          } else {
            reject('Not online');
          }
        });
      } else {
        reject('SDK already loaded');
      }
    });
  }

  private injectSDK(): Promise<any> {

    return new Promise((resolve, reject) => {

      window['mapInit'] = () => {
        this.mapsLoaded = true;
        resolve(true);
      };

      let script = this.renderer.createElement('script');
      script.id = 'googleMaps';

      if (this.apiKey) {
        script.src = 'https://maps.googleapis.com/maps/api/js?key=' + this.apiKey + '&callback=mapInit';
      } else {
        script.src = 'https://maps.googleapis.com/maps/api/js?callback=mapInit';
      }

      this.renderer.appendChild(this.document.body, script);

    });

  }

  private initMap(): Promise<any> {

    return new Promise((resolve, reject) => {

      Geolocation.getCurrentPosition().then((position) => {

        console.log(position);

        let latLng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

        let mapOptions = {
          center: latLng,
          zoom: 15
        };

        this.map = new google.maps.Map(this.element.nativeElement, mapOptions);

        resolve(true);

      }, (err) => {

        reject('Could not initialise map');

      });

    });

  }

  public addMarker(lat: number, lng: number): void {

    let latLng = new google.maps.LatLng(lat, lng);

    let marker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: latLng
    });

    this.markers.push(marker);

  }

  public addInfoWindow(latitude: number, longitude: number, text: string) {
    infoWindow = new google.maps.InfoWindow;
    latLng = new google.maps.LatLng(latitude, longitude);

    infoWindow.setPosition(latLng);
    infoWindow.setContent(text);
    infoWindow.open(this.map);
    this.map.setCenter(latLng);
  }

  public addCustomMarker(latitude: number, longitude: number, img: string) {
    // let latLng: google.maps.LatLng;
    // latLng = new google.maps.LatLng(latitude, longitude);

  }
}
