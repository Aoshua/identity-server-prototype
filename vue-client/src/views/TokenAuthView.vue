<script lang="ts">
	import { defineComponent } from "vue"
    import TokenDetails from '@/components/TokenDetails.vue'

	export default defineComponent({
		name: "HomeView",
        components: { TokenDetails }
	})
</script>

<template>
	<div class="auth-page">
		<h1>Token Authorization</h1>
		<Suspense>
            <TokenDetails />
        </Suspense>
	</div>
</template>

<style scoped>
	.auth-page {
		color: var(--fc-primary);
		padding: 0px 15px;
	}
</style>
