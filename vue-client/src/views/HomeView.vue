<template>
	<div class="home">
		<img alt="Vue logo" src="../assets/logo.png" />
		<div class="mt-3">
		<button type="button" class="btn btn-outline-success" @click="requestResource">Request Protected Resource</button>
	</div>
	</div>
</template>

<script lang="ts">
	import { defineComponent, ref } from "vue"
	import { UserManager, Log } from 'oidc-client-ts'

	export default defineComponent({
		name: "HomeView",
		setup() {
			const requestResource = ref(() => {
				Log.setLogger(console);
				const settings = {
					// TODO: investigate userStore prop 
					authority: "https://localhost:44360/",
					client_id: "Prototype-api",
					redirect_uri: "https://localhost:8080/auth",
					post_logout_redirect_uri: "https://localhost:8080/logged-out",
					response_type: "code",
					scope:"openid profile email Prototype-api"
				};
				const mgr = new UserManager(settings)
				mgr.signinRedirect()
			})

			return {
				requestResource
			}
		}
	})
</script>
