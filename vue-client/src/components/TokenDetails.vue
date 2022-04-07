<script lang="ts">
	import { defineComponent, ref } from "vue"
	import { useOidc } from "@/composables/useOidc"
	import { useRoute } from "vue-router"
    import jwt_decode from "jwt-decode"

	export default defineComponent({
		name: "TokenDetails",
		// The router-view that loads a view with an async setup() needs to be wrapped in <Suspense>
		async setup() {
			const route = useRoute()

			const { mgr } = useOidc()
			await mgr.signinRedirectCallback() // 2. Request Authorization Token

			let user = await mgr.getUser()
			//console.log("user: ", user)

            const accessTokenDecoded = ref(
                user 
                ? JSON.stringify(jwt_decode(user.access_token), null, 4)
                : "Error loading user"
            )

            const userInfo = ref<string | undefined>("Uninitialized")
            const getUserInfo = async () => {
                let response = await fetch('https:localhost:44360/connect/userinfo', {
                    method: 'GET',
                    headers: new Headers({
                        'Authorization': `Bearer ${user?.access_token}`
                    })
                })

                if (response.status == 200) {
                    let content = await response.json()
                    userInfo.value = JSON.stringify(content, null, 4)
                }
                else userInfo.value = user ? user.id_token : "Error loading user"
            }
            getUserInfo()

			return {
                accessTokenDecoded,
				route,
                user,
                userInfo
			}
		}
	})
</script>

<template>
    <h3 class="fc-sec">{{ route.query.code }}</h3>
		<div class="row mx-0 mt-3">
			<div class="col-6">
				<h3>Access Token</h3>
                <p v-if="user" class="text-start text-break fc-sec">{{ user.access_token }}</p>
                <h4 class="text-start">Decoded</h4>
                <pre class="text-start fc-sec">{{accessTokenDecoded}}</pre>
			</div>
			<div class="col-6">
				<h3>ID Token</h3>
                <p v-if="user" class="text-start text-break fc-sec">{{ user.id_token }}</p>
                <h4 class="text-start">Exchanged For</h4>
                <pre class="text-start fc-sec">{{userInfo}}</pre>
			</div>
		</div>
</template>

<style scoped>
    .fc-sec {
        color: var(--fc-secondary);
    }
</style>