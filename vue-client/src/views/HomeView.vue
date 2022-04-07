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
	import {useOidc} from '@/composables/useOidc'

	export default defineComponent({
		name: "HomeView",
		setup() {
			const { mgr } = useOidc()
			const requestResource = ref(async () => {
				
				mgr.signinRedirect() // 1. Make Authorization Request
			})

			return {
				requestResource
			}
		}
	})
</script>
