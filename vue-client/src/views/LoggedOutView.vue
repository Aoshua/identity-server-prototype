<script type="ts">
import { useOidc } from '@/composables/useOidc'
import { defineComponent } from 'vue'


export default defineComponent({
    name: 'LoggedOutView',
    setup() {
        const {mgr} = useOidc()
        
        mgr.signoutRedirectCallback()
    },
})
</script>


<template>
    <p>You have logged out</p>
</template>