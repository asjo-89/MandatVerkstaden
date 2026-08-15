
export const PrimaryButton = ({ btnText, onClick }) => {
  return (
        <>
            <button className="btn btn-primary" onClick={onClick}>{btnText || ""}</button>
        </>
  )
}
