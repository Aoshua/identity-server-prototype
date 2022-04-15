<template>
	<div class="home">
		<img alt="Vue logo" src="../assets/logo.png" />
		<div class="mt-3">
			<button type="button" class="btn btn-outline-success" @click="redirectToLogin">Identity Provider Login</button>
			<br />
			<button type="button" class="mt-3 btn btn-outline-primary" @click="typicalEsriLogin">Typical Esri Login</button>
			<br />
			<button type="button" class="mt-3 btn btn-outline-primary" @click="hiddenEsriLogin">Hidden Esri Login</button>
			<br />
			<button type="button" class="mt-3 btn btn-outline-warning" @click="logout">Logout</button>
		</div>
	</div>
</template>

<script lang="ts">
	import { defineComponent, ref } from "vue"
	import { useIdentityProvider } from "@/composables/useIdentityProvider"
	import { useRouter } from "vue-router"

	export default defineComponent({
		name: "HomeView",
		setup() {
			const { redirectToLogin, logout } = useIdentityProvider()
			const router = useRouter()

			const typicalEsriLogin = ref(() => {
				window.location.href =
					"https://www.arcgis.com/sharing/rest/oauth2/authorize?client_id=ctTqx1iyD7IpPneo&response_type=token&expiration=20160&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Fesri-token-redirect"
			})

			const hiddenEsriLogin = ref(() => {
				router.push({ name: "MapView", query: { mode: "hidden" } })
			})

			return {
				redirectToLogin,
				typicalEsriLogin,
				hiddenEsriLogin,
				logout
			}
		}
	})
</script>
