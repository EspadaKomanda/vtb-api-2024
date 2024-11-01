import Image from "next/image";
import "../styles/main.css";
import NavigationComponent from "../components/navigation_component";
import MainFeatures from "../components/main_features";
import AccumulationComponent from "../components/accumulation_component";
import Carousel from "../components/carousel_offers_component";

export default function Home() {
  return (
    <body>
      <header className="main-bg background-section pt-8 mb-40 very-small:mb-0">
        <NavigationComponent/>
        <MainFeatures />
      </header>
      <main>
        <AccumulationComponent />
        <Carousel />
      </main>
    </body>
  );
}
