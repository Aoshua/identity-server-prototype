import { UserManager, Log } from 'oidc-client-ts'

Log.setLogger(console);

const settings = {
	authority: "https://localhost:44360", //"https://identityprovider.novotx.dev/",
	client_id: "unify-client",
	redirect_uri: "http://localhost:8080/auth",
	post_logout_redirect_uri: "http://localhost:8080/logged-out",
	response_type: "code",
	scope:"openid profile email",
	loadUserInfo: true // calls /connect/userinfo
};

const oidcManager = new UserManager(settings)

export function useIdentityProvider() {
	/** 1. Make Authorization request (this results in a redirect to the IdentityServer login page).
	 *  After the user is authenticated, a redirect will return us to this app, specifically TokenAuthView.vue */
	const redirectToLogin = () => oidcManager.signinRedirect()

	/** 2. Request Authorization Token */
	const handleLoginResponse = async () => {
		await oidcManager.signinRedirectCallback()
		return await oidcManager.getUser()
	}

	const logout = () => oidcManager.signoutRedirect()
	const handleLogoutResponse = () => oidcManager.signoutRedirectCallback()

	return {
		oidcManager,
		redirectToLogin,
		handleLoginResponse,
		logout,
		handleLogoutResponse
	}
}