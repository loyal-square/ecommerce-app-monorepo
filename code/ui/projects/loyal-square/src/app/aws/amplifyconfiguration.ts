const awsmobile = {
    aws_project_region: 'ap-southeast-2',
    aws_cognito_identity_pool_id: 'ap-southeast-2:bcf253af-ec6b-482f-a5cc-fcf5e1741531',
    aws_cognito_region: 'ap-southeast-2',
    aws_user_pools_id: 'ap-southeast-2_EvXq4mfnm',
    aws_user_pools_web_client_id: '2ns6mcdrs7vejicovr8g6obq92',
    oauth: {
      domain: 'ecommerceappclientcc9d3e18-cc9d3e18-dev.auth.ap-southeast-2.amazoncognito.com',
      scope: ['phone', 'email', 'openid', 'profile', 'aws.cognito.signin.user.admin'],
      redirectSignIn: window.location.origin + '/profile',
      redirectSignOut: window.location.origin,
      responseType: 'code'
    },
    federationTarget: 'COGNITO_USER_POOLS',
    aws_cognito_username_attributes: ['EMAIL'],
    aws_cognito_social_providers: ['GOOGLE'],
    aws_cognito_signup_attributes: ['EMAIL'],
    aws_cognito_mfa_configuration: 'OFF',
    aws_cognito_mfa_types: ['SMS'],
    aws_cognito_password_protection_settings: {
      passwordPolicyMinLength: 8,
      passwordPolicyCharacters: []
    },
    aws_cognito_verification_mechanisms: ['EMAIL']
  };
  
  export default awsmobile;