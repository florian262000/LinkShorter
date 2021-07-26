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

// Custom rule to validate URLs

extend("url", (v: string) => {
  if (v.match(/https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z0-9()]{1,63}\b([-a-zA-Z0-9()@:%_+.~#?&//=]*)/)) {
    return true;
  }

  return "URL is not valid!";
});

// Localization for vee-validate
localize("en", en);

// Get current domain name
Vue.prototype.$domainName = window.location.origin;

Vue.use(VueCookies);

Vue.config.productionTip = false;

new Vue({
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
