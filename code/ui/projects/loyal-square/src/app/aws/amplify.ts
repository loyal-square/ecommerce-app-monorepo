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
} from 'aws-amplify/auth';

export async function signUp(password: string, email: string) {
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

export async function resendConfirmationCode(username: string) {
  try {
    await resendSignUpCode({ username });
    console.log('code resent successfully');
  } catch (err) {
    console.log('error resending code: ', err);
  }
}

export async function confirmSignUp(username: string, code: string) {
  try {
    await confirmSignUpAws({ username, confirmationCode: code });
    await handleAutoSignIn();
  } catch (error) {
    console.log('error confirming sign up', error);
  }
}

async function handleAutoSignIn() {
  try {
    const signInOutput = await autoSignIn();
    // handle sign-in steps
  } catch (error) {
    console.log(error);
  }
}

export async function signIn(username: string, password: string) {
  try {
    const user = await signInAws({ username, password });
    return user;
  } catch (error) {
    console.log('error signing in', error);
    return null;
  }
}

export async function signOut() {
  try {
    await signOutAws();
  } catch (error) {
    console.log('error signing out: ', error);
  }
}

export async function signOutGlobal() {
  try {
    await signOutAws({ global: true });
  } catch (error) {
    console.log('error signing out: ', error);
  }
}

export async function handleResetPassword(username: string) {
  try {
    const output = await resetPassword({ username });
    handleResetPasswordNextSteps(output);
  } catch (error) {
    console.log(error);
  }
}

function handleResetPasswordNextSteps(output: ResetPasswordOutput) {
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

export async function handleConfirmResetPassword({
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
