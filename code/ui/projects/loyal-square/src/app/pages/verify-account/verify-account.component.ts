import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { confirmSignUp } from '../../aws/amplify';

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

  constructor(public formBuilder: FormBuilder, public router: Router) {}

  public async verifyUser(): Promise<void> {
    const { email, code } = this.verifyForm.value;

    if (!code || !email) {
      console.log('unable to verify user due to null input(s)');
      return;
    }
    try {
      await confirmSignUp(email, code);
    } catch (err) {
      alert(err);
    }
  }
}
