import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    isLoggedIn: false,
    username: "",
  },
  mutations: {
    setIsLoggedIn(state, value) {
      state.isLoggedIn = value;
    },

    setUsername(state, username) {
      state.username = username;
    },
  },
  actions: {},
  modules: {},
  getters: {

    getIsLoggedIn: (state) => {
      return state.isLoggedIn;
    },
    getUsername: (state) => {
      return state.username;
    },

  },
});
