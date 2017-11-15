import { observable } from "mobx";
import Dinosaur from "../../model/Dinosaur";

class CreateDinosaurStore {
    @observable dinosaur: Dinosaur = new Dinosaur();
}

export default new CreateDinosaurStore();