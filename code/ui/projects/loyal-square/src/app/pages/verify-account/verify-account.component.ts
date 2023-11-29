import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-verify-account',
  templateUrl: './verify-account.component.html',
  styleUrls: ['./verify-account.component.css'],
})
export class VerifyAccountComponent {
  public verifyForm = this.formBuilder.group({
    code: '',
    email: '',
  });

  constructor(
    public formBuilder: FormBuilder,
    public router: Router,
    public authService: AuthService
  ) {}

  public async verifyUser(): Promise<void> {
    const { email, code } = this.verifyForm.value;

    if (!code || !email) {
      console.log('unable to verify user due to null input(s)');
      return;
    }
    try {
      await this.authService.confirmSignUp(email, code);
    } catch (err) {
      alert(err);
    }
  }
}
