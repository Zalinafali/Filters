   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   :xctk)http://schemas.xceed.com/wpf/xaml/toolkit     5   	     � Title$$  Computer Graphics 1: Project 1��5	   	   $
��1000��60   $
��1200��6>   $	��800��6K   $	��450��6Z   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $	��300 �6   .+�5   
   �� $��3��6   $
��Auto=�6'   .��5      �� 
���   �$��1q�6)   $��3��6=   $��3q�6M   ._�5      �  � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5       $
��Auto �6(   �� 5!      $
��Auto �6(   �� 5"      $
��Auto �6(   �� 5#      $
��Auto �6(   �� 5$      $
��Auto �6(   �� 5&      $
��Auto �6(   �� 5'      $
��Auto �6(   �� 5(      $
��Auto �6(   �� 5)      $
��Auto �6(   �� 5*      $
��Auto �6(   �� 5,      $��* �6(   .+�5/      �� -   5        $��	loadImage��5/   *   $��0��6   
�������
���   �$��1q�6f   $��5q�50      .��6;   
Load image52      �� 54      -   5        $��	saveImage��54   *   $��1��6   
�������
���   �$��1q�6f   $��5q�55      {�.  .��6R   
Save image57      �� 59      -   5        $��reverseChanges��59   *   $��2��6   
�������
���   �$��1q�6k   $��5q�5:      {�.  .��6W   Reverse changes5<      �� 5?      $��3��6   $��3q�6)   $
��Bold"�64   .��6F   Function filters:6[   � 5A      $��4��6    5B      �� 5C      $
��Auto �6/   �� 5D      $��* �6/   .+�5F      �� $��0��6    $��10 0 0 0q�60   .��6B   
Inverse:6L   �� 5G      -   5        $��inverseCheckbox��5G   3   $��1��6#   $��0 0 20 0q�6J   $��Center=�6\   $��Right=�6w   {�.  6�   5H      � 5J      $��5��6    5K      �� 5L      $
��Auto �6/   �� 5M      $��* �6/   .+�5O      �� $��0��6    $��10 0 0 0q�60   .��6B   Brightness correction:6Z   �� 5P      -   5        $��brightnessCheckbox��5P   3   $��1��6#   $��0 0 20 0q�6M   $��Center=�6_   $��Right=�6z   {�.  6�   5Q      � 5S      $��6��6    5T      �� 5U      $
��Auto �6/   �� 5V      $��* �6/   .+�5X      �� $��0��6    $��10 0 0 0q�60   .��6B   Contrast enhancement:6Y   �� 5Y      -   5        $��contrastCheckbox��5Y   3   $��1��6#   $��0 0 20 0q�6K   $��Center=�6]   $��Right=�6x   {�.  6�   5Z      � 5\      $��7��6    5]      �� 5^      $
��Auto �6/   �� 5_      $��* �6/   .+�5a      �� $��0��6    $��10 0 0 0q�60   .��6B   Gamma correction:6U   �� 5b      -   5        $��gammaCheckbox��5b   3   $��1��6#   $��0 0 20 0q�6H   $��Center=�6Z   $��Right=�6u   {�.  6�   5c      �� 5e      $��8��6    $��3q�6-   �� 5h      $��9��6   $��3q�6)   $
��Bold"�64   .��6F   Convolution filters:6^   � 5j      $��10��6    5k      �� 5l      $
��Auto �6/   �� 5m      $��* �6/   .+�5o      �� $��0��6    $��10 0 0 0q�60   .��6B   Blur:6K   �� 5p      -   5        $��blurCheckbox��5p   3   $��1��6#   $��0 0 20 0q�6G   $��Center=�6Y   $��Right=�6t   {�.  6�   5q      � 5s      $��11��6    5t      �� 5u      $
��Auto �6/   �� 5v      $��* �6/   .+�5x      �� $��0��6    $��10 0 0 0q�60   .��6B   Gaussian blur:6T   �� 5y      -	   5        $��gaussianCheckbox��5y   3   $��1��6#   $��0 0 20 0q�6K   $��Center=�6]   $��Right=�6x   {�.  6�   5z      � 5|      $��12��6    5}      �� 5~      $
��Auto �6/   �� 5      $��* �6/   .+�5�      �� $��0��6    $��10 0 0 0q�60   .��6B   
Sharpen:6N   �� 5�      -
   5        $��sharpenCheckbox��5�   3   $��1��6#   $��0 0 20 0q�6J   $��Center=�6\   $��Right=�6w   {�.  6�   5�      � 5�      $��13��6    5�      �� 5�      $
��Auto �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6    $��10 0 0 0q�60   .��6B   Edge detection:6U   �� 5�      -   5        $��edgeCheckbox��5�   3   $��1��6#   $��0 0 20 0q�6G   $��Center=�6Y   $��Right=�6t   {�.  6�   5�      � 5�      $��14��6    5�      �� 5�      $
��Auto �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6    $��10 0 0 0q�60   .��6B   	Emboss:6M   �� 5�      -   5        $��embossCheckbox��5�   3   $��1��6#   $��0 0 20 0q�6I   $��Center=�6[   $��Right=�6v   {�.  6�   5�      �� 5�      $��15��6    $��3q�6.   �� 5�      $��16��6   $��3q�6*   $
��Bold"�65   .��6G    Create/Edit functional filter:6g   �� 5�      $��17��6   $	��260��6+   $	��260��67   
�������$��1q�6W   ._�5�      �� -   5        $��canvas��5�   ^   $��1q�6!   $	��256��6,   $	��256��68   
�������6�   5�      � 5�      $��18��6   $��3q�6)    5�      �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   .+�5�      �� $��0��6    $��5 1 1 1q�60   .��6A   X:6E   �� 5�      -   5        $��pointX��5�   "   $��1��60   $��1q�6@    �� TextAlignment$ Center=�6K   $��20��6b   $��30��6n   .��6y   06|   �� 5�      $��2��6    $��5 1 1 1q�60   .��6A   Y:6E   �� 5�      -   5        $��pointY��5�   "   $��3��60   $��1q�6@   $ Center=�6K   $��20��6b   $��30��6n   .��6y   06|   �� 5�      $��4��6!   $��10 1 1 1q�61   $��45��6D   .��6P   Save6X   �� 5�      $��5��6!   $��5 1 1 1q�61   $��45��6C   .��6O   Delete6Y   �� 5�      -   5         ��Name$ 
moveButton��5�   !   $��6��65   $��5 1 1 1q�6E   $��45��6V   .��6z   Move6�   5�      �� 5�      -   5        $��moveMenu��5�      $��19��6-   $��3q�6;   $	��200��6F   $��60��6R   $y�Hidden=�6^   
���   �$	��0.5q�6�   ._�5�      � $��3q�6   $��Center=�6*    5�      �� 5�   "   $
��Auto �63   �� 5�   "   $
��Auto �63   �� 5�   "   $
��Auto �63   �� 5�   "   $
��Auto �63   �� 5�   "   $
��Auto �63   �� 5�   "   $
��Auto �63    5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   .+�5�      �� $��0��6$   $��0��64   .��6A   From:6H   �� 5�      $��1��6$   $��0��64   $��3 1 1 1q�6A   .��6R   X:6V   �� 5�      -   5        $��	moveFromX��5�   &   $��2��67   $��0��6G   $��1q�6T   $ Center=�6_   $��20��6v   $��30��6�   .��6�   06�   �� 5�      $��3��6$   $��0��64   $��3 1 1 1q�6A   .��6R   Y:6V   �� 5�      -   5        $��	moveFromY��5�   &   $��4��67   $��0��6G   $��1q�6T   $ Center=�6_   $��20��6v   $��30��6�   .��6�   06�   �� 5�      $��0��6$   $��1��64   .��6A   To:6F   �� 5�      $��1��6$   $��1��64   $��3 1 1 1q�6A   .��6R   X:6V   �� 5�      -   5        $��moveToX��5�   &   $��2��65   $��1��6E   $��1q�6R   $ Center=�6]   $��20��6t   $��30��6�   .��6�   06�   �� 5�      $��3��6$   $��1��64   $��3 1 1 1q�6A   .��6R   Y:6V   �� 5�      -   5        $��moveToY��5�   &   $��4��65   $��1��6E   $��1q�6R   $ Center=�6]   $��20��6t   $��30��6�   .��6�   06�   5�      5�      5�      5�      5�      � 5�   
   $��0��6   .+�5�      �� $��3q�6   $��1q�6    
�������._�5�      �� -   5        $��originalImage��5�      $��10q�6-   5�      5�      � 5�   
   $��1��6   .+�5�      �� $��3q�6   $��1q�6    
�������._�5�      �� -   5        $��filteredImage��5�      $��10q�6-   5�      5�      5�      5�      