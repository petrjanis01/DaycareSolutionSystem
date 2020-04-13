import { Component, OnInit, ViewChild, ElementRef, Renderer2, AfterContentInit, AfterViewInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LoginDTO } from 'src/app/api/generated';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements AfterViewInit {
  @ViewChild('usernameContainer') usernameContainer: ElementRef;
  @ViewChild('passwordContainer') passwordContainer: ElementRef;
  @ViewChild('usernameInput') usernameInput: ElementRef;
  @ViewChild('passwordInput') passwordInput: ElementRef;
  @ViewChild('loginBg') loginBg: ElementRef;

  constructor(private renderer: Renderer2, private auth: AuthenticationService, private router: Router) { }

  ngAfterViewInit() {
    console.log(this.loginBg);
    this.renderer.setStyle(this.loginBg.nativeElement, 'background', 'url("/assets/img/login-bg.jpg")');
  }

  public onUsernameBlur() {
    this.renderer.removeClass(this.usernameContainer.nativeElement, 'focused');
  }

  public onUsernameFocus() {
    this.renderer.addClass(this.usernameContainer.nativeElement, 'focused');
  }

  public onPasswordBlur() {
    this.renderer.removeClass(this.passwordContainer.nativeElement, 'focused');
  }

  public onPasswordFocus() {
    this.renderer.addClass(this.passwordContainer.nativeElement, 'focused');
  }

  public async login() {
    let loginDto: LoginDTO = {
      username: this.usernameInput.nativeElement.value,
      password: this.passwordInput.nativeElement.value,
      isManagementSite: true
    };

    let loginSuccesful = await this.auth.logIn(loginDto);

    if (loginSuccesful) {
      this.router.navigate(['']);
    }
  }

}
