import { Component, AfterViewInit, ElementRef, ViewChild, Renderer2, Inject } from '@angular/core';
import { LoginDTO } from '../../api/generated/model/loginDTO';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { NavController } from '@ionic/angular';
import { Router } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements AfterViewInit {
  @ViewChild('loginBg') loginBg: ElementRef;

  public username: string;
  public password: string;

  constructor(
    private auth: AuthenticationService,
    private nav: NavController,
    private router: Router,
    private renderer: Renderer2,
    @Inject(APP_BASE_HREF) private baseHref: string) { }

  ngAfterViewInit() {
    let bgImgUrl = `${this.baseHref != null ? this.baseHref : ''}/assets/img/login-background.jpg`;
    this.renderer.setStyle(this.loginBg.nativeElement, 'background', `url('${bgImgUrl}')`);
  }

  public async login() {
    if (!this.password && this.username === '#setup#') {
      this.openSetup();
      return;
    }

    let loginDto: LoginDTO = {
      username: this.username,
      password: this.password,
      isManagementSite: false
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
