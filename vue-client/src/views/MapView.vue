<template>
	<div class="map" :id="mapId" style="height: calc(100vh - 84px);"></div>
</template>

<script lang="ts">
	import { defineComponent, onMounted } from "vue"
	import { useState } from "@/composables/useState"
	import Map from "@arcgis/core/WebMap"
	import MapView from "@arcgis/core/views/MapView"
	import SimpleRenderer from "@arcgis/core/renderers/SimpleRenderer"
	import PictureMarkerSymbol from "@arcgis/core/symbols/PictureMarkerSymbol"
	import LabelClass from "@arcgis/core/layers/support/LabelClass"
	import TextSymbol from "@arcgis/core/symbols/TextSymbol"
	import FeatureLayer from "@arcgis/core/layers/FeatureLayer"
	import IdentityManager from "@arcgis/core/identity/IdentityManager"
	import { useRoute } from "vue-router"
	// import { IDENTITY_URL } from "@/composables/useIdentityProvider"
	//import { IdentityManagerRegisterTokenProperties } from '@arcgis/core/identity/IdentityManager'

	export default defineComponent({
		name: "MapView",
		setup() {
			const { state } = useState()
			const route = useRoute()

			let token = ""
			if (route.query.mode == 'typical') token = state.value.esriToken!
			else token = state.value.loggedUser!.access_token!

			console.log(`${route.query.mode == 'typical' ? 'esri' : 'internal'} token: ${token}`)

			var tokenProps = {
				server: "https://elements.maps.arcgis.com/sharing/rest/oauth2/authorize",
				userId: "",
				token: token,
				ssl: true,
				expires: 7200
			}
			IdentityManager.registerToken(tokenProps)

			const mapId = "testEsriMap"

			onMounted(() => {
				const map = new Map({ basemap: "streets-night-vector" })

				const trailheadsLayer = new FeatureLayer({
					url: `https://utility.arcgis.com/usrsvcs/servers/be398022876d4182857d82b892fb7743/rest/services/Hosted/Parks/FeatureServer/0`,
					renderer: new SimpleRenderer({
						symbol: new PictureMarkerSymbol({
							url: "http://static.arcgis.com/images/Symbols/NPS/npsPictograph_0231b.png",
							width: "18px",
							height: "18px"
						})
					}),
					labelingInfo: [
						new LabelClass({
							labelPlacement: "above-center",
							labelExpressionInfo: {
								expression: "$feature.TRL_NAME"
							},
							symbol: new TextSymbol({
								color: "#fff",
								haloColor: "#5e8d74",
								haloSize: "2px",
								font: {
									size: "12px",
									family: "Noto Sans",
									style: "italic",
									weight: "normal"
								}
							})
						})
					]
				})
				map.add(trailheadsLayer)

				new MapView({
					center: [-111.608,40.164],
					container: mapId,
					map,
					zoom: 13
				})
			})

			return {
				mapId
			}
		}
	})
</script>

<style scoped>
	@import "https://js.arcgis.com/4.19/@arcgis/core/assets/esri/themes/dark/main.css";

	.map {
		height: 100%;
		margin: 0;
		padding: 0;
		width: 100%;
	}
</style>
