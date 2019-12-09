import Vue from "vue";
import Router from "vue-router";

import ROverview from "./components/routes/ROverview.vue";

export enum R {
  HOME = "/",
}

export const NAV_ITEMS_ENGLISH = [
  {icon: "home", text: "Overview", route: R.HOME},
];

const routes = [
  {path: R.HOME, component: ROverview},
];

export default new Router({
  routes
});

Vue.use(Router);
