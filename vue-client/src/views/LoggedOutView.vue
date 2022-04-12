<script type="ts">
import { useIdentityProvider } from '@/composables/useIdentityProvider'
import { defineComponent } from 'vue'


export default defineComponent({
    name: 'LoggedOutView',
    setup() {
        const {handleLogoutResponse} = useIdentityProvider()
        handleLogoutResponse()
    },
})
</script>

<template>
    <p>You have logged out</p>
</template>