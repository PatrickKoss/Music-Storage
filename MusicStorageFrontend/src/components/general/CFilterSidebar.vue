<template>
  <v-layout row wrap>
    <v-flex>
      <v-navigation-drawer
        :clipped="$vuetify.breakpoint.mdAndUp"
        :dark="darkeningGeneral"
        app
        temporary
        id="c-navigationdrawer"
        v-model="drawer"
        :right="true"
        :width="400"
      >
        <v-list dense>
          <v-layout row wrap>
            <h2 style="padding: 15px">Filter:</h2>
          </v-layout>
          <v-layout row wrap>
            <v-autocomplete
              v-model="interpretFilter"
              label="Interpret"
              :items="appStore.state.interprets"
              class="itemFilter"
            ></v-autocomplete>
          </v-layout>
          <v-layout row wrap>
            <v-select
              v-model="genreFilter"
              label="Genre"
              :items="genre"
              class="itemFilter"
            ></v-select>
          </v-layout>
          <v-layout row wrap id="bottom">
            <v-flex
              @click="resetFilter()"
            >
              <v-btn>Clear Filter</v-btn></v-flex
            >
            <v-flex @click="drawer = false" style="right: 5px; position: fixed">
              <v-btn>Apply Filter</v-btn></v-flex
            >
          </v-layout>
        </v-list>
      </v-navigation-drawer>
    </v-flex>
    <v-btn class="ma-2" tile large icon @click="drawer = true">
      <v-icon>filter_list</v-icon>
    </v-btn>
  </v-layout>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { StateModule, AppStore } from "../../store/AppStore";
import { VueStateField } from "../../store/State";
import { Watch } from "vue-property-decorator";
import IFilterMusicFile from "../../model/IFilterMusicFile";
import {config} from "../../config/index";

@Component
export default class CFilterSidebar extends Vue {
  @VueStateField(StateModule.GENERAL)
  public darkeningGeneral: boolean;

  appStore = AppStore;

  interpretFilter = "";
  genreFilter = "";
  genre = config.genre;

  drawer = false;

  public created() {
    AppStore.commit("loadInterprets");
  }

  resetFilter() {
    this.interpretFilter = "";
    this.genreFilter = "";
    AppStore.commit("resetFilterMusicFile");
    AppStore.commit("loadMusicFiles");
  }

  @Watch("interpretFilter")
  public __filterInterpret() {
    AppStore.commit("setFilterMusicFile", {
      interpret: this.interpretFilter,
      genre: this.genreFilter
    } as IFilterMusicFile);
    AppStore.commit("loadMusicFiles");
  }

  @Watch("genreFilter")
  public __filterGenre() {
    AppStore.commit("setFilterMusicFile", {
      interpret: this.interpretFilter,
      genre: this.genreFilter
    } as IFilterMusicFile);
    AppStore.commit("loadMusicFiles");
  }
}
</script>

<style scoped>
#bottom {
  position: fixed;
  bottom: 0;
}
.itemFilter {
  padding: 15px;
}
</style>
