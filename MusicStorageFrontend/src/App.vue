<template>
  <v-app id="app">
    <c-toolbar></c-toolbar>
    <v-layout align-start="align-start" ref="contentRef">
      <c-content></c-content>
      <v-snackbar :bottom="bottom" :class="{successMessage: !errorBool, errorMessages: errorBool}"
                  :dark="darkeningGeneral"
                  :timeout="timeout" multi-line
                  v-model="showError">{{error}}
      </v-snackbar>
    </v-layout>
  </v-app>
</template>

<script lang="ts">
  import Vue from "vue";
  import {Component, Watch} from "vue-property-decorator";
  import {StringsUtilEnglish} from "./assets/StringsUtilEnglish";
  import CContent from "./components/general/CContent.vue";
  import CToolBar from "./components/general/CToolbar.vue";
  import {AppStore, StateModule} from "./store/AppStore";
  import {VueStateField} from "./store/State";
  @Component({
    name: "app",
    components: {
      "c-toolbar": CToolBar,
      "c-content": CContent,
    }
  })
  export default class App extends Vue {
    @VueStateField(StateModule.GENERAL)
    public darkeningGeneral: boolean;
    @VueStateField(StateModule.GENERAL)
    public error: string;
    @VueStateField(StateModule.GENERAL)
    public timeout: number;
    @VueStateField(StateModule.GENERAL)
    public showError: boolean;
    @VueStateField(StateModule.GENERAL)
    public bottom: boolean;
    @VueStateField(StateModule.GENERAL)
    public errorBool: boolean;

    created() {
      //set up the theme
      let theme = localStorage.getItem("s");
      if (theme !== null) {
        if (theme === "Light") {
          this.darkeningGeneral = false;
        }
        if (theme === "Dark") {
          this.darkeningGeneral = true;
        }
      } else {
        this.darkeningGeneral = false;
      }
    }

    @Watch("error")
    __showError() {
      if (this.error !== "") {
        this.showError = true;
      }
    }
  }
</script>

<style lang="css">
  /* Import vuetify styles */
  @import "~vuetify/dist/vuetify.min.css";
  @import "~vue-d3-network/dist/vue-d3-network.css";
  .successMessage {
    font-size: 200%;
  }
  .errorMessage {
    font-size: 200%;
  }
</style>