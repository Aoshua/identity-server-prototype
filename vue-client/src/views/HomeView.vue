<template>
	<div class="home">
		<img alt="Vue logo" src="../assets/logo.png" />
		<div class="mt-3">
			<button type="button" class="btn btn-outline-success" @click="redirectToLogin">Request Protected Resource</button>
			<br />
			<button type="button" class="mt-3 btn btn-outline-danger" @click="clearSiteData">Clear Site Data</button>
		</div>
	</div>
</template>

<script lang="ts">
	import { defineComponent, ref } from "vue"
	import { useIdentityProvider } from "@/composables/useIdentityProvider"

	export default defineComponent({
		name: "HomeView",
		setup() {
			const { redirectToLogin } = useIdentityProvider()

			const clearSiteData = ref(() => {
				localStorage.clear()
				sessionStorage.clear()
				document.cookie = "idsrv.session=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
			})

			return {
				clearSiteData,
				redirectToLogin
			}
		}
	})
</script>
