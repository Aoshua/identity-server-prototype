<script lang="ts">
	import { defineComponent, ref } from "vue"
	import { useRoute } from "vue-router"
    import jwt_decode from "jwt-decode"
	import { useIdentityProvider } from "@/composables/useIdentityProvider"
	import { useState } from "@/composables/useState"

	export default defineComponent({
		name: "TokenDetails",
		// The router-view that loads a view with an async setup() needs to be wrapped in <Suspense>
		async setup() {
			const route = useRoute()
			const {setLoggedUser} = useState()
			const { handleLoginResponse } = useIdentityProvider()			

			let user = await handleLoginResponse()
			console.log("user: ", user)
			setLoggedUser(user)

            const accessTokenDecoded = ref(
                user 
                ? JSON.stringify(jwt_decode(user.access_token), null, 4)
                : "Error loading user"
            )

			return {
                accessTokenDecoded,
				route,
                user
			}
		}
	})
</script>

<template>
    <h3 class="fc-sec">{{ route.query.code }}</h3>
		<div class="row mx-0 mt-3">
			<div class="col-6 text-start scroller">
				<h3 class="text-center">Access Token</h3>
                <p v-if="user" class="text-break fc-sec">{{ user.access_token }}</p>
                <h4>Decoded</h4>
                <pre class="fc-sec">{{accessTokenDecoded}}</pre>
			</div>
			<div class="col-6 text-start scroller">
				<h3 class="text-center">ID Token</h3>
                <p v-if="user" class="text-break fc-sec">{{ user.id_token }}</p>
                <h4 class="">Profile</h4>
                <pre v-if="user" class="text-start fc-sec">{{ JSON.stringify(user.profile, null, 4) }}</pre>
			</div>
		</div>
</template>

<style scoped>
    .fc-sec {
        color: var(--fc-secondary);
    }
	.scroller {
		max-height: calc(100vh - 205px); 
		overflow-y: scroll;
	}
</style>