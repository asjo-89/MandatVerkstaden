import { AiOutlineDashboard } from "react-icons/ai";
import { TbBuildingCommunity, TbChartBar, TbUsersGroup } from "react-icons/tb";
import { GrDocumentTest } from "react-icons/gr";
import { LuMenu } from "react-icons/lu";

export default function Menu() {
  return (
    <>
        <nav className="navigation">
          <h1><a href="#" className="logo-text manrope-extra-bold">Val<span>Kvoten</span></a></h1>
          
          {/* Mobile and tablet menu */}
          <button className="hamburger-menu" popoverTarget="hamburger" type="button" aria-label="Open menu"><LuMenu /></button>
          <ul popover="auto" id="hamburger" className="menu-popover">
            <li><a href="#" className="nav-opt manrope-bold"><AiOutlineDashboard /> Översikt</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbBuildingCommunity /> Kommuner</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbUsersGroup /> Partier</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbChartBar  /> Valresultat</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><GrDocumentTest /> Scenarier</a></li>
          </ul>

          {/* Desktop menu */}
          <ul className="sidebar-menu">
            <li><a href="#" className="nav-opt manrope-bold"><AiOutlineDashboard /> Översikt</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbBuildingCommunity /> Kommuner</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbUsersGroup /> Partier</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbChartBar  /> Valresultat</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><GrDocumentTest /> Scenarier</a></li>
          </ul>
        </nav>
    </>
  )
}
