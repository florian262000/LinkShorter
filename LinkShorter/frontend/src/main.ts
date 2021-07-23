import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import store from "./store";

import VueCookies from "vue-cookies";
import { extend, localize } from "vee-validate";
import en from "vee-validate/dist/locale/en.json";
import * as rules from "vee-validate/dist/rules";

// Setup vee-validate ruleset
for (const [rule, validation] of Object.entries(rules)) {
  extend(rule, {
    ...validation,
  });
}

// Localization for vee-validate
localize("en", en);


Vue.use(VueCookies);

Vue.config.productionTip = false;

new Vue({
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
