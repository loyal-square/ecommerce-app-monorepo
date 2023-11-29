import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

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

  constructor(
    public formBuilder: FormBuilder,
    public router: Router,
    public authService: AuthService
  ) {}

  public async registerUser(): Promise<void> {
    const { password, email, confirmPassword } = this.registerForm.value;

    if (!password || !email || !confirmPassword) {
      console.log('unable to register user due to null input(s)');
      return;
    }
    try {
      await this.authService.signUp(password, email);
      this.router.navigate(['/verify-account']);
    } catch (err) {
      alert(err);
    }
  }
}
