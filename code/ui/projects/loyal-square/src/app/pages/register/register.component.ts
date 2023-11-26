import { Component } from '@angular/core';
import { signUp } from '../../aws/amplify';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  public registerForm = this.formBuilder.group({
    password: '',
    email: '',
    confirmPassword: '',
  });

  constructor(public formBuilder: FormBuilder, public router: Router) {}

  public async registerUser(): Promise<void> {
    const { password, email, confirmPassword } = this.registerForm.value;

    if (!password || !email || !confirmPassword) {
      console.log('unable to register user due to null input(s)');
      return;
    }
    try {
      await signUp(password, email);
      this.router.navigate(['/verify-account']);
    } catch (err) {
      alert(err);
    }
  }
}
