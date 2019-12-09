<template>
  <v-toolbar :clipped-left="$vuetify.breakpoint.mdAndUp" :dark="darkeningGeneral" app fixed id="c-toolbar">
    <!-- this component creates the toolbar -->
    <v-toolbar-title justify-center>Music Storage</v-toolbar-title>
    <v-spacer></v-spacer>
    <v-menu :dark="darkeningGeneral" bottom="bottom" left="left" offset-y="offset-y" open-on-hover="open-on-hover">
      <v-btn icon large slot="activator">
        <v-avatar size="32px" tile>
          <v-icon>settings</v-icon>
        </v-avatar>
      </v-btn>
      <v-list :three-line="true">
        <v-list-tile @click="changeTheme()">
          <v-switch :label="'Dark theme'" v-model="themeSwitch"></v-switch>
        </v-list-tile>
      </v-list>
    </v-menu>
  </v-toolbar>
</template>

<script lang="ts">
  import Vue from "vue";
  import {Component, Watch} from "vue-property-decorator";
  import {StateModule} from "../../store/AppStore";
  import {VueStateField} from "../../store/State";
  @Component
  export default class CToolBar extends Vue {
    @VueStateField(StateModule.GENERAL)
    public darkeningGeneral: boolean;
    
    public theme: string = "Light";
    public themeSwitch: boolean = false;

    /**
     * this method sets the theme out of the local storage
     */
    public mounted() {
      let theme = localStorage.getItem("theme");
      if (theme !== null) {
        if (theme === "Light") {
          this.theme = theme;
          this.themeSwitch = false;
        }
        if (theme === "Dark") {
          this.theme = theme;
          this.themeSwitch = true;
        }
      } else {
        this.theme = "Light";
        this.themeSwitch = false;
      }
    }
    /**
     * once the switch for change the theme is triggered adjust the bool and the local storage for the theme
     * @private
     */
    @Watch("theme")
    public __changeTheme() {
      this.darkeningGeneral = this.theme === "Dark";
      if (this.darkeningGeneral === false) {
        localStorage.setItem("theme", "Light");
      } else {
        localStorage.setItem("theme", "Dark");
      }
    }
    /**
     * change the theme
     */
    public changeTheme() {
      if (this.theme === "Light") {
        this.theme = "Dark";
        this.themeSwitch = true;
      } else {
        this.theme = "Light";
        this.themeSwitch = false;
      }
    }
  }
</script>

<style>
</style>