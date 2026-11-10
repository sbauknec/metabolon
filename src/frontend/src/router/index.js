import { createRouter, createWebHistory } from 'vue-router'

import LoginView from '@/views/LoginView.vue'
import DashboardView from '@/views/DashboardView.vue'
import RoomListView from '@/views/RoomListView.vue'
import DeviceListView from '@/views/DeviceListView.vue'
import ItemListView from '@/views/ItemListView.vue'

import RoomView from '@/views/RoomView.vue'

import PageLayout from '@/layouts/PageLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'login',
      component: LoginView
    },
    {
      path: '/',
      component: PageLayout,
      meta: { requiresAuth: true },
      children: [
        {
          path: '/dashboard/',
          name: 'dashboard',
          component: DashboardView
        },
        {
          path: '/roomlist/',
          name: 'roomlist',
          component: RoomListView
        },
        {
          path: '/devicelist/',
          name: 'devicelist',
          component: DeviceListView
        },
        {
          path: '/itemlist/',
          name: 'itemlist',
          component: ItemListView
        },
        {
          path: '/room/:id',
          name: 'room',
          component: RoomView
        }
      ]
    }
  ]
})

export default router
