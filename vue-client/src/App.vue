<template>
	<nav>
		<router-link to="/">Home</router-link> |
		<router-link to="/about">About</router-link>
	</nav>
	<!-- The suspense tag is needed to load view with async setup -->
	<Suspense>
		<router-view />
	</Suspense>
</template>

<style>
	:root {
		--fc-primary: whitesmoke;
		--fc-secondary: #f5f5f5a3;
	}

	body {
		background-color: #2c3e50 !important;
		color: var(--fc-primary);
		height: 100vh;
	}

	#app {
		font-family: Avenir, Helvetica, Arial, sans-serif;
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		text-align: center;
	}

	nav {
		padding: 30px;
	}

	nav a {
		font-weight: bold;
		color: whitesmoke;
	}

	nav a.router-link-exact-active {
		color: #42b983;
	}
</style>
