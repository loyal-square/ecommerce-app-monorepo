import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  signIn as signInAws,
  signUp as signUpAws,
  resendSignUpCode,
  confirmSignUp as confirmSignUpAws,
  autoSignIn,
  signOut as signOutAws,
  getCurrentUser,
  resetPassword,
  confirmResetPassword,
  ResetPasswordOutput,
  ConfirmResetPasswordInput,
  fetchAuthSession,
  JWT,
} from 'aws-amplify/auth';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public accessToken: string | undefined = undefined;
  constructor(public router: Router) {}
  public async init() {
    this.accessToken = await this.getCurrentSession();
  }
  public async signUp(password: string, email: string) {
    const { isSignUpComplete, userId, nextStep } = await signUpAws({
      username: email,
      password,
      options: {
        userAttributes: {
          email,
        },
        // optional
        autoSignIn: true, // or SignInOptions e.g { authFlowType: "USER_SRP_AUTH" }
      },
    });
  }
  public async resendConfirmationCode(username: string) {
    try {
      await resendSignUpCode({ username });
      console.log('code resent successfully');
    } catch (err) {
      console.log('error resending code: ', err);
    }
  }

  public async confirmSignUp(username: string, code: string) {
    try {
      await confirmSignUpAws({ username, confirmationCode: code });
      await this.handleAutoSignIn();
    } catch (error) {
      console.log('error confirming sign up', error);
    }
  }

  async handleAutoSignIn() {
    try {
      const signInOutput = await autoSignIn();
      this.router.navigate(['/profile']);
      // handle sign-in steps
    } catch (error) {
      console.log(error);
    }
  }

  async signIn(username: string, password: string) {
    try {
      const user = await signInAws({ username, password });
      this.accessToken = await this.getCurrentSession();
      this.router.navigate(['/profile']);
    } catch (error) {
      console.log('error signing in', error);
    }
  }
  private async getCurrentSession() {
    try {
      const { accessToken } = (await fetchAuthSession()).tokens ?? {};
      return accessToken?.toString();
    } catch (err) {
      console.log('error getting session:', err);
      return undefined;
    }
  }
  async signOut() {
    try {
      await signOutAws();
    } catch (error) {
      console.log('error signing out: ', error);
    }
  }

  async signOutGlobal() {
    try {
      await signOutAws({ global: true });
    } catch (error) {
      console.log('error signing out: ', error);
    }
  }

  async handleResetPassword(username: string) {
    try {
      const output = await resetPassword({ username });
      this.handleResetPasswordNextSteps(output);
    } catch (error) {
      console.log(error);
    }
  }

  handleResetPasswordNextSteps(output: ResetPasswordOutput) {
    const { nextStep } = output;
    switch (nextStep.resetPasswordStep) {
      case 'CONFIRM_RESET_PASSWORD_WITH_CODE':
        const codeDeliveryDetails = nextStep.codeDeliveryDetails;
        console.log(
          `Confirmation code was sent to ${codeDeliveryDetails.deliveryMedium}`
        );
        // Collect the confirmation code from the user and pass to confirmResetPassword.
        break;
      case 'DONE':
        console.log('Successfully reset password.');
        break;
    }
  }

  async handleConfirmResetPassword({
    username,
    confirmationCode,
    newPassword,
  }: ConfirmResetPasswordInput) {
    try {
      await confirmResetPassword({ username, confirmationCode, newPassword });
    } catch (error) {
      console.log(error);
    }
  }
}
