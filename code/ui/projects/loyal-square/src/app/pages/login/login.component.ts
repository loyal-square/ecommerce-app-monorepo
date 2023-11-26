import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { signIn } from '../../aws/amplify';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  public loginForm = this.formBuilder.group({
    password: '',
    email: '',
  });

  constructor(public formBuilder: FormBuilder, public router: Router) {}

  public async login(): Promise<void> {
    const { email, password } = this.loginForm.value;

    if (!password || !email) {
      console.log('unable to verify user due to null input(s)');
      return;
    }
    try {
      await signIn(email, password);
    } catch (err) {
      alert(err);
    }
  }
}
