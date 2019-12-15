<template>
  <v-layout id="root">
    <v-dialog
      :dark="darkeningGeneral"
      max-width="500"
      v-model="dialogD"
      :persistent="true"
    >
      <v-card :dark="darkeningGeneral">
        <v-card-title class="headline" primary-title
          >Add new music file</v-card-title
        >

        <v-card-text>
          <form ref="form">
            <v-layout row wrap>
              <v-text-field
                      v-model="musicFile.Title"
                      label="Title"
                      ref="title"
                      :error-messages="errorsTitle"
              />
            </v-layout>
            <v-layout row wrap>
              <v-combobox
                      v-model="musicFile.Interpret"
                      :items="appStore.state.interprets"
                      label="Select an interpret or create new one"
                      :error-messages="errorsInterpret"
                      ref="interpret"
                      :readonly="interpretMode"
              />
            </v-layout>
            <v-layout row wrap>
              <v-select
                      v-model="musicFile.Genre"
                      label="Select genre"
                      :items="genre"
                      :error-messages="errorsGenre"
                      ref="genre"
              />
            </v-layout>
          </form>
        </v-card-text>

        <v-divider/>

        <v-card-actions>
          <v-btn @click="cancel()" flat>Cancel</v-btn>
          <v-spacer/>
          <v-btn @click="addMusicFile()" color="primary" flat v-if="!editMode"
            >Add music file
          </v-btn>
          <v-btn @click="addMusicFile()" color="primary" flat v-if="editMode"
            >Edit music file
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch } from "vue-property-decorator";
import { StateModule, AppStore } from "../../store/AppStore";
import { VueStateField } from "../../store/State";
import VeeValidate, { validate } from "vee-validate";
import { IMusicFileWithoutIDs } from "../../model/IMusicFileWithoutIDs";
import { extend } from "vee-validate";
import { required } from "vee-validate/dist/rules";
import { config } from "../../config/index";
import { MusicFileRestClient } from "../../model/MusicFileRestClient";
import { IMusicFile } from "../../model/IMusicFile";
import HelperFunctions from "../../model/HelperFunctions";

@Component({
  components: {}
})
export default class CAddMusicFileDialog extends Vue {
  @VueStateField(StateModule.GENERAL)
  public darkeningGeneral: boolean;

  @VueStateField(StateModule.GENERAL)
  public timeout: number;

  @Prop() public dialog: boolean;
  @Prop() public editMode: boolean;
  @Prop() public musicFileProp: IMusicFileWithoutIDs;
  @Prop() public interpretMode: boolean;

  firstValidate = false;

  appStore = AppStore;
  helper: any = new HelperFunctions();

  public dialogD: boolean = this.dialog;
  errorsTitle = [];
  errorsInterpret = [];
  errorsGenre = [];

  $refs!: {
    title: any;
    genre: any;
    interpret: any;
    form: any;
  };

  musicFile = {
    Title: "",
    Interpret: "",
    Genre: "",
    ID: null
  } as IMusicFileWithoutIDs;

  genre = config.genre;

  public created() {
    AppStore.commit("loadInterprets");
  }

  async addMusicFile() {
    this.firstValidate = true;
    let validation = true;
    if (this.musicFile.Title.length === 0) {
      this.errorsTitle.push("This field is required.");
      validation = false;
    }
    if (this.musicFile.Interpret.length === 0) {
      this.errorsInterpret.push("This field is required.");
      validation = false;
    }
    if (this.musicFile.Genre.length === 0) {
      this.errorsGenre.push("This field is required.");
      validation = false;
    }
    if (validation) {
      try {
        this.$refs.interpret.selectItem();
        if (this.editMode) {
          let musicFile: IMusicFile = Object.assign(
            AppStore.state.musicFiles.find(
              element => element.ID === this.musicFile.ID
            )
          );
          let index = AppStore.state.musicFiles.findIndex(
            musicFile => musicFile.Interpret.Name === this.musicFile.Interpret
          );
          musicFile.Interpret.ID = -1;
          if (index !== -1) {
            musicFile.Interpret.ID =
              AppStore.state.musicFiles[index].Interpret.ID;
          }
          musicFile.Title.Name = this.musicFile.Title;
          musicFile.Interpret.Name = this.musicFile.Interpret;
          musicFile.Genre = this.musicFile.Genre;
          const text = (await MusicFileRestClient.updateMusicFile(musicFile))
            .data;
          this.helper.showError(text, this.timeout, false);
        } else {
          let musicFile = {
            ID: -1,
            Title: { ID: -1, Name: "" },
            Interpret: { ID: -1, Name: "" },
            Genre: ""
          } as IMusicFile;
          let index = AppStore.state.musicFiles.findIndex(
            element => element.Interpret.Name === this.musicFile.Interpret
          );
          if (index !== -1) {
            musicFile.Interpret = AppStore.state.musicFiles[index].Interpret;
          } else {
            musicFile.Interpret.Name = this.musicFile.Interpret;
          }
          musicFile.Title.Name = this.musicFile.Title;
          musicFile.Genre = this.musicFile.Genre;
          const text = (await MusicFileRestClient.createMusicFile(musicFile))
            .data;
          this.helper.showError(text, this.timeout, false);
        }
        AppStore.commit("loadMusicFiles");
        AppStore.commit("loadInterprets");
      } catch (e) {
        console.log(e);
      }
      this.dialogD = false;
      this.firstValidate = false;
    }
  }

  cancel() {
    this.dialogD = false;
    this.firstValidate = false;
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

  @Watch("musicFile.Title", { immediate: true, deep: true })
  public __musicTitle() {
    if (this.firstValidate) {
      if (this.musicFile.Title.length > 0) {
        this.errorsTitle = [];
      }
    }
  }

  @Watch("musicFile.Interpret", { immediate: true, deep: true })
  public __interpret() {
    if (this.firstValidate) {
      if (this.musicFile.Interpret.length > 0) {
        this.errorsInterpret = [];
      }
    }
  }

  @Watch("musicFile.Genre", { immediate: true, deep: true })
  public __genre() {
    if (this.firstValidate) {
      if (this.musicFile.Genre.length > 0) {
        this.errorsGenre = [];
      }
    }
  }

  @Watch("musicFileProp", { immediate: true, deep: true })
  public __setMusicFile() {
    this.musicFile = {...this.musicFileProp};
    this.errorsTitle = [];
    this.errorsInterpret = [];
    this.errorsGenre = [];
  }
}
</script>

<style scoped></style>
