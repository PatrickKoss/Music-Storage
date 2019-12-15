import Vue from "vue";
import Router from "vue-router";

import RInterprets from "./components/routes/RInterprets.vue";
import RInterpretsDetail from "./components/routes/RInterpretsDetail.vue";
import ROverview from "./components/routes/ROverview.vue";

export enum R {
  HOME = "/",
  INTERPRETS = "/interprets",
  INTERPRETSDETAIL = "/interprets/:id"
}

export const NAV_ITEMS_ENGLISH = [
  {icon: "home", text: "Overview", route: R.HOME},
  {icon: "home", text: "Interprets", route: R.INTERPRETS},
  {icon: "home", text: "InterpretsDetail", route: R.INTERPRETSDETAIL}
];

const routes = [
  {path: R.HOME, component: ROverview},
  {path: R.INTERPRETS, component: RInterprets},
  {path: R.INTERPRETSDETAIL, component: RInterpretsDetail}
];

export default new Router({
  routes
});

Vue.use(Router);
