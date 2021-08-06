<template>
  <v-container>
    <v-row>
      <v-col>
        <a target="_blank" :href="`${$domainName}/${userShortlink.ShortPath}`">
          <h1>{{ `${$domainName}/${userShortlink.ShortPath}` }}</h1>
        </a>
        <br />
        <h2>
          Links to: <a target="_blank" :href="userShortlink.TargetUrl"> {{ userShortlink.TargetUrl }} </a>
        </h2>
        <br />
        <h2>Used {{ userShortlink.ClickCounter }} times</h2>
        <br />
        <h2>Created at {{ userShortlink.TimeStamp }}</h2>
      </v-col>
      <v-col cols="1">
        <v-btn fab depressed color="error" icon @click="deleteShortlink">
          <v-icon>mdi-trash-can</v-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import { mapMutations, mapGetters } from "vuex";
import UserShortlink from "@/interfaces/userShortlink";
import axios from "axios";

export default Vue.extend({
  props: {
    userShortlink: {
      type: Object as () => UserShortlink,
      default: {},
    },
  },
  methods: {
    ...mapGetters(["getShortlinks"]),
    ...mapMutations(["setShortlinks"]),
    async deleteShortlink() {
      try {
        const response = await axios.delete("api/link/add", {
          withCredentials: true,
          data: {
            shortPath: this.userShortlink.ShortPath,
          },
        });

        if (response.status !== 200) {
          throw response;
        }

        this.setShortlinks(this.getShortlinks().filter((s: UserShortlink) => s !== this.userShortlink));
      } catch (e) {
        if (e.status < 500) {
          if (e.status === 401) {
            console.log("Delete shortlink - auth error");
          } else {
            console.log("Delete shortlink - unexcpected error");
          }
        } else {
          console.log("Delete shortlink - server error");
        }
      }
    },
  },
});
</script>

<style scoped>
/* unvisited link */
a:link,
a:visited {
  color: grey;
  text-decoration: none;
}

/* mouse over link */
a:hover {
  color: dodgerblue;
}
</style>
