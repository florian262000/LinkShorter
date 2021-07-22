import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import store from "./store";
import { extend } from "vee-validate";
import * as rules from "vee-validate/dist/rules";

for (const [rule, validation] of Object.entries(rules)) {
  extend(rule, {
    ...validation,
  });
}

Vue.config.productionTip = false;

new Vue({
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
