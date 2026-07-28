import { AiOutlineDashboard } from "react-icons/ai";
import { TbBuildingCommunity, TbChartBar, TbUsersGroup } from "react-icons/tb";
import { GrDocumentTest } from "react-icons/gr";
import { LuMenu } from "react-icons/lu";

function App() {

  return (
    <>
      <main className="manrope-regular">
        <header className="header-container">
          <div className="logo-container">
            <h1 className="logo-text manrope-extra-bold">
              <a href="#">Val<span>Kvoten</span></a>
            </h1>
          </div>

          <div className="menu-container">
            <button className="hamburger-menu" popoverTarget="hamburger" type="button" aria-label="Open menu"><LuMenu /></button>
            <nav className="navigation">
              <ul popover="auto" id="hamburger" className="menu-popover" >
                <li><a href="#" className="nav-opt manrope-bold"><AiOutlineDashboard /> Översikt</a></li>
                <li><a href="#" className="nav-opt manrope-bold"><TbBuildingCommunity /> Kommuner</a></li>
                <li><a href="#" className="nav-opt manrope-bold"><TbUsersGroup /> Partier</a></li>
                <li><a href="#" className="nav-opt manrope-bold"><TbChartBar  /> Valresultat</a></li>
                <li><a href="#" className="nav-opt manrope-bold"><GrDocumentTest /> Scenarier</a></li>
              </ul>
            </nav>
          </div>
        </header>
      </main>
    </>
  )
}

export default App
