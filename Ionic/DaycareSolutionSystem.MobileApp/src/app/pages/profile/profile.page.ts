import { Component } from '@angular/core';
// import { Base64 } from '@ionic-native/base64/ngx';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage {

  public async onPictureChanged(event) {
    if (event.target.files && event.target.files[0]) {
      let picture = event.target.files[0];
    }
  }
}
