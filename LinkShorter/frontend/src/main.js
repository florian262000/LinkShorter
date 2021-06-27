import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import vuetify from './plugins/vuetify'

Vue.config.productionTip = false
Vue.config.devtools = true

import axios from 'axios'
Vue.use(require('vue-cookies'))
Vue.prototype.$http = axios
new Vue({
  vuetify,
  render: h => h(App)
}).$mount('#app')
