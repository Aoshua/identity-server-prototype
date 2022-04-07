import { UserManager, Log } from 'oidc-client-ts'

Log.setLogger(console);

const settings = {
	authority: "https://localhost:44360/",
	client_id: "vue-client",
	redirect_uri: "http://localhost:8080/auth",
	post_logout_redirect_uri: "http://localhost:8080/logged-out",
	response_type: "code",
	scope:"openid profile email"
};

const mgr = new UserManager(settings)

export function useOidc() {
    return {
		mgr
	}
}