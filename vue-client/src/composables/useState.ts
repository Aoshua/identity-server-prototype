import { readonly, ref, watch } from 'vue'
import { IState } from '@/models/state'
import { User } from 'oidc-client-ts/dist/types/oidc-client-ts'

/** If value is defined and not null, returns value; else returns default value */
const definedOrDefault = <T>(value: T, defaultValue: NonNullable<T>) => {
	const v = value as NonNullable<T>
	return v != null ? v : defaultValue
}

const isEqual = <T>(obj1: T, obj2: T) => JSON.stringify(obj1) == JSON.stringify(obj2)

const getState = () => JSON.parse(localStorage.getItem('state') || '{}')
const defaultState = (key: string, defaultValue: any) => definedOrDefault(getState()[key], defaultValue)

const state = ref<IState>({
	loggedUser: defaultState('loggedUser', undefined),
	esriToken: defaultState('esriToken', undefined)
})

export interface ILoginResult {
	success: boolean,
	errorMessage: string
}

export function useState() {
	watch(state, () => {
		const storageState = getState()
		if (!isEqual(storageState, state.value)) localStorage.setItem('state', JSON.stringify(state.value))
	}, { immediate: true, deep: true })
	window.onstorage = () => {
		const storageState = getState()
		if (!isEqual(storageState, state.value)) Object.keys(storageState).forEach((key: string) => state.value[key] = storageState[key])
	}

	const setLoggedUser = (user: User | null) => {
		state.value.loggedUser = user
	}

	const setEsriToken = (token: string) => {
		state.value.esriToken = token
	}
	
	return {
		state: readonly(state),
		setLoggedUser,
		setEsriToken
	}
}
