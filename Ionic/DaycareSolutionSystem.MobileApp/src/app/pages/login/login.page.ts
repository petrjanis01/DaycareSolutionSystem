import { Component } from '@angular/core';
import { TestService, RegisteredActionsService } from 'src/app/api/generated';
import { LoginDTO } from '../../api/generated/model/loginDTO';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { NavController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage {
  public username: string;
  public password: string;

  constructor(private auth: AuthenticationService, private nav: NavController, private reg: RegisteredActionsService) { }

  public async login() {
    let loginDto: LoginDTO = {
      username: this.username,
      password: this.password
    };

    let loginSuccesful = await this.auth.logIn(loginDto);

    if (loginSuccesful) {
      this.nav.navigateRoot('/tabs');
    }
  }
}
