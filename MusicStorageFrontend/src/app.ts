// Internal global dependencies
import "core-js/features/array";
import "core-js/features/string";

// Vue framework
import Vue from "vue";
// Vuetify UI components
import Vuetify from "vuetify"
// Kick start the main Vue component
import App from "./App.vue";
// VueRouter
import Router from "./router";
// Vuex store
import {AppStore} from "./store/AppStore";
import Axios from "axios";

Axios.defaults.baseURL = process.env.API_ENDPOINT;

Vue.use(Vuetify, {
  options: {
    themeVariations: ["content"]
  },
  theme: {}
});

Vue.config.productionTip = false;

window.onload = () => {
  const app = new Vue({
    components: {
      App
    },
    el: ".app",
    render: (h) => h(App),
    router: Router,
    store: AppStore
  });
};
