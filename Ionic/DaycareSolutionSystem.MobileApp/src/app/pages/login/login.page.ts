import { Component } from '@angular/core';
import { LoginDTO } from '../../api/generated/model/loginDTO';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { NavController } from '@ionic/angular';
import { Router } from '@angular/router';
import { BaseUrlService } from 'src/app/services/base-url.service';
import { TestService } from 'src/app/api/generated';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage {
  public username: string;
  public password: string;

  constructor(private auth: AuthenticationService, private nav: NavController, private router: Router) { }

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
  public openSetup() {
    this.router.navigate(['setup']);
  }
}
