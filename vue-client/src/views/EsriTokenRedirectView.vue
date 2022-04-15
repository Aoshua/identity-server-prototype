<template>
	<div></div>
</template>

<script lang="ts">
	import { defineComponent } from 'vue'
	import { useRouter } from 'vue-router'
	import { useState } from '@/composables/useState'

	export default defineComponent({
		name: 'EsriTokenRedirectView',
		setup() {
			const router = useRouter()
			const { setEsriToken } = useState()

			let params = window.location.hash.slice(1).split('&')
			let accessToken = params.filter(x => x.includes('access_token'))[0].split('=')[1]
			setEsriToken(accessToken)
			router.push({ name: 'MapView', query: { mode: 'typical'} })
		}
	})
</script>
