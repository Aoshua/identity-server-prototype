import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/about',
    name: 'about',
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  },
  {
    path: '/auth',
    name: 'TokenAuthView',
    component: () => import(/* webpackChunkName: "TokenAuthView" */ '../views/TokenAuthView.vue')
  },
  {
    path: '/logged-out',
    name: 'LoggedOutView',
    component: () => import(/* webpackChunkName: "LoggedOutView" */ '../views/LoggedOutView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
