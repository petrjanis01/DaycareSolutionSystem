import { Component, ViewChild } from '@angular/core';
import { GoogleMapComponent } from '../shared/google-map/google-map.component';
import { Plugins, GeolocationPosition } from '@capacitor/core';

import html2canvas from 'html2canvas';

const { Geolocation } = Plugins;

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  @ViewChild(GoogleMapComponent, { static: true }) mapComponent: GoogleMapComponent;

  currentLocation: GeolocationPosition;
  capturedImage;

  constructor() {
    this.loadCurrentLocation();
  }

  loadCurrentLocation() {
    Geolocation.getCurrentPosition().then((position) => {
      this.currentLocation = position;
    });
  }

  testMarker() {
    let center = this.mapComponent.map.getCenter();
    this.mapComponent.addMarker(center.lat(), center.lng());
  }

  showInfoWindow() {
    this.mapComponent.addInfoWindow(49.595219, 17.231734, 'Home sweet home');
  }

  showCustomMarker() {
    this.getMarkerAsImg('#marker1');
  }

  getMarkerAsImg(id: string) {
    html2canvas(document.querySelector(id)).then(canvas => {
      /// document.body.appendChild(canvas);
      this.capturedImage = canvas.toDataURL();
      console.log('canvas.toDataURL() -->' + this.capturedImage);
      // this will contain something like (note the ellipses for brevity), console.log cuts it off 
      // "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAa0AAAB3CAYAAACwhB/KAAAXr0lEQVR4Xu2dCdiNZf7HP/ZQkpQtaUxDjYYoTSYlURMhGlmKa..."
    });
  }
}
