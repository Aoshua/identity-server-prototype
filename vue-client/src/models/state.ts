import { User } from 'oidc-client-ts/dist/types/oidc-client-ts'

export interface IState extends Record<string, any> {
	loggedUser?: User | null
	esriToken?: string | null
}