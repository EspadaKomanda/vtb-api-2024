import Image from "next/image";
import "../styles/main.css";
import NavigationComponent from "../components/navigation_component";
import MainFeatures from "../components/main_features";
import AccumulationComponent from "../components/accumulation_component";
import Carousel from "../components/carousel_offers_component";

export default function Home() {
  return (
    <>
      <header className="main-bg background-section pt-8">
        <NavigationComponent/>
        <MainFeatures />
        <AccumulationComponent />
        <Carousel />
      </header>
    </>
  );
}
