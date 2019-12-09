<template>
  <!-- This component is dialog where you can confirm that you want to delete a system or scenario -->
  <v-layout id="root">
    <v-dialog :dark="darkeningGeneral" max-width="500" v-model="dialogD">
      <v-card :dark="darkeningGeneral">
        <v-card-title class="headline" primary-title>Delete</v-card-title>

        <v-card-text>Do you really want to delete {{ component }}?</v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-btn @click="dialogD = false" flat>Cancel</v-btn>
          <v-spacer></v-spacer>
          <v-btn @click="deleteComponent()" color="primary" flat>Delete </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch } from "vue-property-decorator";
import { VueStateField } from "../../store/State";
import { MusicFileRestClient } from "../../model/MusicFileRestClient";
import HelperFunctions from "../../model/HelperFunctions";
import { StateModule, AppStore } from "../../store/AppStore";

@Component({
  components: {}
})
export default class CDeleteDialog extends Vue {
  @VueStateField(StateModule.GENERAL)
  public darkeningGeneral: boolean;

  @VueStateField(StateModule.GENERAL)
  public timeout: number;

  @Prop() public dialog: boolean;
  @Prop() public id: number;
  @Prop() component: string;
  public dialogD: boolean = this.dialog;
  helper: any = new HelperFunctions();

  async deleteComponent() {
    const text = (await MusicFileRestClient.deleteMusicFile(this.id)).data;
    this.helper.showError(text, this.timeout, false);
    this.dialogD = false;
    AppStore.commit("loadMusicFiles");
    AppStore.commit("loadInterprets");
  }
  /**
   * to avoid warnings in the console, a component intern dialog variable needs to be set like the property
   * @private
   */
  @Watch("dialog")
  public __dialog() {
    this.dialogD = this.dialog;
  }
  /**
   * once the dialogD is false, the dialog in the parent should also be false, so message has to be sent to tbe parent
   * @private
   */
  @Watch("dialogD")
  public __dialogD() {
    if (!this.dialogD) {
      this.$emit("closed-Dialog", this.dialogD);
    }
  }
}
</script>

<style scoped></style>
