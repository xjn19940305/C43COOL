// 视图组件
const view = {
  tabs: () => import('@/layouts/tabs'),
  blank: () => import('@/layouts/BlankView'),
  page: () => import('@/layouts/PageView')
}

// 路由组件注册
const routerMap = {
  login: {
    authority: '*',
    path: '/login',
    component: () => import('@/pages/login')
  },
  root: {
    path: '/',
    name: '首页',
    redirect: '/login',
    component: view.tabs
  },
  dashboard: {
    name: '目视板',
    path: 'dashboard',
    component: () => import('@/pages/public/dashboard')
  },
  exp403: {
    authority: '*',
    name: 'exp403',
    path: '403',
    component: () => import('@/pages/exception/403')
  },
  exp404: {
    name: 'exp404',
    path: '404',
    component: () => import('@/pages/exception/404')
  },
  exp500: {
    name: 'exp500',
    path: '500',
    component: () => import('@/pages/exception/500')
  },
  auth: {
    name: '系统权限',
    component: view.page
  },
  role: {
    name: '角色管理',
    path: 'role',
    component: () => import('@/pages/auth/role')
  },
  route: {
    name: '模块管理',
    path: 'route',
    component: () => import('@/pages/auth/route')
  },
  account: {
    name: '用户管理',
    path: 'account',
    component: () => import('@/pages/auth/account')
  },
  resource: {
    name: '资源',
    component: view.page
  },
  category: {
    name: '分类管理',
    path: 'category',
    component: () => import('@/pages/resource/category')
  },
  article: {
    name: '文章管理',
    path: 'article',
    component: () => import('@/pages/resource/article')
  },
  articleDetail: {
    name: '文章详情',
    path: '/resource/articleDetail',
    component: () => import('@/pages/resource/articleDetail'),
    invisible: true
  },
}
export default routerMap

